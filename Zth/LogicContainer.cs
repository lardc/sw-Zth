using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Zth.Components;
using Zth.IO;
using Zth.VM;

namespace Zth
{
    internal class LogicContainer
    {
        //Адаптер
        private readonly IOAdapter Adapter;
        //Узел Zth
        private readonly ushort Node;
        //VM верхней панели
        private readonly TopPanelVm TopPanelVm;
        //VM нижней панели
        private readonly BottomPanelVM BottomPanelVM;
        //Итерация опроса
        private int PollIteration;

        public LogicContainer(ushort node = 10)
        {
            Adapter = new IOAdapter();
            Node = node;
            TopPanelVm = ((MainWindow)App.Current.MainWindow).TopPanelVM;
            BottomPanelVM = ((MainWindow)App.Current.MainWindow).BottomPanelVM;
        }

        public bool IsRunning //Флаг старта измерения
        {
            get; set;
        }

        public DateTime StartTime //Начало измерений
        {
            get; set;
        }

        public CommonVM CommonVM //VM общих данных
        {
            get; set;
        }

        public Chart Chart //График для отображения
        {
            get; set;
        }

        public async void EnablePower() //Запуск установки
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    App.Logger.Info("Enabling power");
                    CallAction(ACT_ENABLE_POWER);
                    //Ожидание запуска установки
                    while ((HWDeviceState)ReadRegister(REG_DEV_STATE, true) != HWDeviceState.DS_READY)
                        Thread.Sleep(50);
                    App.Logger.Info("Power enabled");
                    ResultsCycle();
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Enabling power failed");
                    MessageBox.Show("Не удалось запустить комплекс.\nПроверьте статус подключения к аппаратной части.", "Ошибка");
                    //Закрытие приложения
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        App.Current.MainWindow.Close();
                    });
                }
            });
        }

        public async void DisablePower() //Выключение установки
        {
            await Task.Run(() =>
            {
                try
                {
                    IsRunning = false;
                    App.Logger.Info("Disabling power");
                    CallAction(ACT_DISABLE_POWER);
                    App.Logger.Info("Power disabled");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Disabling power failed");
                }
            });
        }

        public async void StartZthLongImpulse(uint pulseWidth) //Запуск измерения Zth длинный импульс
        {
            await Task.Run(() =>
            {
                try
                {
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 16);
                    ushort LowByte = (ushort)(pulseWidth & 0xFFFF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Starting Zth long impulse");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Zth long impulse started");
                    IsRunning = true;
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Zth long impulse");
                }
            });
        }

        public async void UpdateZthLongImpulse(TypeDevice device, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay, uint pulseWidth) //Обновление параметров измерения Zth длинный импульс
        {
            await Task.Run(() =>
            {
                try
                {
                    //Обновление общих параметров
                    UpdateCommonData(device, gateParameter, measuringCurrent, heatingCurrent, measurementDelay);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 16);
                    ushort LowByte = (ushort)(pulseWidth & 0xFFFF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Updating Zth long impulse");
                    CallAction(ACT_UPDATE);
                    App.Logger.Info("Zth long impulse updated");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not update Zth long impulse");
                }
            });
        }

        public async void ReadEndpointsZthLongImpulse() //Чтение эндпоинтов Zth длинный импульс
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Reading endpoints for Zth long impulse");
                    //Очистка графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.ClearChart(true);
                    });
                    //Время (мкс)
                    double Duration = 200;
                    ReadEndpoints(Duration);
                    App.Logger.Info("Reading endpoints for Zth long impulse finished");
                    //Выравнивание графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.AdjustChart();
                    });
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not read Zth long impulse endpoints");
                }
            });
        }

        public async void StartZthSequence(ushort firstPulseWidth, uint lastPulseWidth, ushort pause) //Запуск измерения Zth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Setting the 1st pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MIN, firstPulseWidth);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(lastPulseWidth >> 16);
                    ushort LowByte = (ushort)(lastPulseWidth & 0xFFFF);
                    App.Logger.Info("Setting the last pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_ZTH_PAUSE, pause);
                    App.Logger.Info("Starting Zth sequence");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Zth sequence started");
                    IsRunning = true;
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Zth sequence");
                }
            });
        }

        public async void ReadEndpointsZthSequence() //Чтение эндпоинтов Zth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Reading endpoints for Zth sequence");
                    //Очистка графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.ClearChart(true);
                    });
                    //Время (мкс)
                    double Duration = ((ZthPulseSequenceVM)CommonVM).FirstPulseDuration * 1000;
                    ReadEndpoints(Duration);
                    App.Logger.Info("Reading endpoints for Zth sequence finished");
                    //Выравнивание графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.AdjustChart();
                    });
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not read Zth sequence endpoints");
                }
            });
        }

        public async void StartRthSequence(uint pulseWidth, ushort pause) //Запуск измерения Rth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 16);
                    ushort LowByte = (ushort)(pulseWidth & 0xFFFF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_PAUSE, pause);
                    App.Logger.Info("Starting Rth sequence");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Rth sequence started");
                    IsRunning = true;
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Rth sequence");
                }
            });
        }

        public async void UpdateRthSequence(TypeDevice device, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay, uint pulseWidth, ushort pause) //Обновление параметров измерения Rth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    //Обновление общих параметров
                    UpdateCommonData(device, gateParameter, measuringCurrent, heatingCurrent, measurementDelay);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 16);
                    ushort LowByte = (ushort)(pulseWidth & 0xFFFF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_PAUSE, pause);
                    App.Logger.Info("Updating Rth sequence");
                    CallAction(ACT_UPDATE);
                    App.Logger.Info("Rth sequence updated");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not update Rth sequence");
                }
            });
        }

        public async void StartGraduation(uint pulseWidth, ushort pause, ushort temperature) //Запуск измерения Градуировка
        {
            await Task.Run(() =>
            {
                try
                {
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 16);
                    ushort LowByte = (ushort)(pulseWidth & 0xFFFF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_PAUSE, pause);
                    App.Logger.Info("Setting max temperature");
                    WriteRegister(REG_T_MAX, temperature);
                    App.Logger.Info("Starting graduation");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Graduation started");
                    IsRunning = true;
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start graduation");
                }
            });
        }

        public async void UpdateGraduation(ushort[] heatingCurrent, ushort temperature) //Обновление параметров измерения Градуировка
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Setting heating current");
                    WriteRegister(REG_I_WIDTH_LESS_2MS, heatingCurrent[0]);
                    WriteRegister(REG_I_WIDTH_LESS_10MS, heatingCurrent[1]);
                    WriteRegister(REG_I_WIDTH_ABOVE_10MS, heatingCurrent[2]);
                    App.Logger.Info("Setting max temperature");
                    WriteRegister(REG_T_MAX, temperature);
                    App.Logger.Info("Updating graduation");
                    CallAction(ACT_UPDATE);
                    App.Logger.Info("Graduation updated");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not update graduation");
                }
            });
        }

        public async void ReadEndpointsGraduation() //Чтение эндпоинтов Градуировка
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Reading endpoints for graduation");
                    //Очистка графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.ClearChart(true);
                    });
                    //Время (мкс)
                    double Duration = 200;
                    ReadEndpoints(Duration);
                    App.Logger.Info("Reading endpoints for graduation finished");
                    //Выравнивание графиков
                    App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Chart.AdjustChart();
                    });
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not read graduation endpoints");
                }
            });
        }

        public async Task PrepareForMeasure(TypeDevice device, TypeCooling cooling, WorkingMode mode, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay) //Подготовка к измерению
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    App.Logger.Info("Setting device type");
                    WriteRegister(REG_DUT_TYPE, (ushort)device);
                    App.Logger.Info("Setting cooling type");
                    WriteRegister(REG_COOLING_MODE, (ushort)cooling);
                    App.Logger.Info("Setting working mode");
                    WriteRegister(REG_MODE, (ushort)mode);
                    switch (device)
                    {
                        case TypeDevice.Bipolar:
                            App.Logger.Info("Setting gate current");
                            WriteRegister(REG_GATE_CURRENT, gateParameter);
                            break;
                        case TypeDevice.Igbt:
                            App.Logger.Info("Setting IGBT gate voltage");
                            WriteRegister(REG_IGBT_V_GATE, gateParameter);
                            break;
                    }
                    App.Logger.Info("Setting measuring current");
                    WriteRegister(REG_MEASURING_CURRENT, measuringCurrent);
                    App.Logger.Info("Setting heating current");
                    WriteRegister(REG_I_WIDTH_LESS_2MS, heatingCurrent[0]);
                    WriteRegister(REG_I_WIDTH_LESS_10MS, heatingCurrent[1]);
                    WriteRegister(REG_I_WIDTH_ABOVE_10MS, heatingCurrent[2]);
                    App.Logger.Info("Setting measurement delay");
                    WriteRegister(REG_MEASUREMENT_DELAY, measurementDelay);
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not write measurement parameters");
                }
            });
        }

        public void UpdateCommonData(TypeDevice device, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay) //Обновление общих параметров
        {
            switch (device)
            {
                case TypeDevice.Bipolar:
                    App.Logger.Info("Setting gate current");
                    WriteRegister(REG_GATE_CURRENT, gateParameter);
                    break;
                case TypeDevice.Igbt:
                    App.Logger.Info("Setting IGBT gate voltage");
                    WriteRegister(REG_IGBT_V_GATE, gateParameter);
                    break;
            }
            App.Logger.Info("Setting measuring current");
            WriteRegister(REG_MEASURING_CURRENT, measuringCurrent);
            App.Logger.Info("Setting heating current");
            WriteRegister(REG_I_WIDTH_LESS_2MS, heatingCurrent[0]);
            WriteRegister(REG_I_WIDTH_LESS_10MS, heatingCurrent[1]);
            WriteRegister(REG_I_WIDTH_ABOVE_10MS, heatingCurrent[2]);
            App.Logger.Info("Setting measurement delay");
            WriteRegister(REG_MEASUREMENT_DELAY, measurementDelay);
        }

        public void ReadEndpoints(double duration) //Общий алгоритм чтения эндпоинтов
        {
            //Температура корпуса
            IList<ushort> TempCase1Endpoints = ReadArray(EP_T_CASE1);
            IList<ushort> TempCase2Endpoints = ReadArray(EP_T_CASE2);
            //Температура охладителя
            IList<ushort> TempCool1Endpoints = ReadArray(EP_T_COOL1);
            IList<ushort> TempCool2Endpoints = ReadArray(EP_T_COOL2);
            //ТЧП
            IList<ushort> TempTspEndpoints = ReadArray(EP_TSP);
            //Отключение лишних графиков
            CommonVM.HeatingCurrentIsVisibly = false;
            CommonVM.HeatingPowerIsVisibly = false;
            //Добавление на графики
            for (int i = 0; i < TempCase1Endpoints.Count; i++)
            {
                //Длительность
                double Timestamp = duration / 1000000.0;
                //Температура корпуса
                double TempCase1 = TempCase1Endpoints[i] / 10.0;
                CommonVM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(Timestamp, TempCase1));
                double TempCase2 = TempCase2Endpoints[i] / 10.0;
                if (CommonVM.CathodeBodyTemperatureIsVisibly)
                    CommonVM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(Timestamp, TempCase2));
                //Температура охладителя
                double TempCool1 = TempCool1Endpoints[i] / 10.0;
                CommonVM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(Timestamp, TempCool1));
                double TempCool2 = TempCool2Endpoints[i] / 10.0;
                if (CommonVM.CathodeCoolerTemperatureIsVisibly)
                    CommonVM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(Timestamp, TempCool2));
                //ТЧП
                double Tsp = TempTspEndpoints[i];
                if (CommonVM.TemperatureSensitiveParameterIsVisibly == true)
                    CommonVM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(Timestamp, Tsp));
                //Итерация времени
                duration += 100 * CountDurationMultiplier(duration);
            }
        }

        public double CountDurationMultiplier(double Duration) //Коэффициент умножения длительности
        {
            //Коэффициент умножения
            double Multiplier;
            //Мкс
            if (Duration < 1000)
                Multiplier = 1;
            //Мс
            else if (Duration < 10000)
                Multiplier = 10;
            //10 мс
            else if (Duration < 100000)
                Multiplier = 100;
            //100 мс
            else if (Duration < 1000000)
                Multiplier = 1000;
            //Cек
            else if (Duration < 10000000)
                Multiplier = 10000;
            //10 сек
            else if (Duration < 100000000)
                Multiplier = 100000;
            //100 сек
            else
                Multiplier = 1000000;
            return Multiplier;
        }

        public async void ResultsCycle() //Цикл чтения результатов
        {
            while (true)
                await ReadResults();
        }

        public async Task ReadResults() //Чтение результатов
        {
            await Task.Run(() =>
            {
                //Длительность измерения в секундах
                double Duration = 0;
                try
                {
                    //Проверка состояния
                    CheckDeviceState();
                    //Греющий ток
                    double HeatingCurrent = ReadRegister(REG_ACTUAL_I_DUT, true) / 10.0;
                    TopPanelVm.HeatingCurrent = HeatingCurrent;
                    //Греющая мощность
                    ushort Whole = ReadRegister(REG_ACTUAL_P_DUT_WHOLE, true);
                    ushort Fractional = ReadRegister(REG_ACTUAL_P_DUT_FRACT, true);
                    double HeatingPower = Whole + Fractional / 100.0;
                    //Напряжение на приборе
                    double Udut = ReadRegister(REG_ACTUAL_U_DUT, true);
                    TopPanelVm.Udut = Udut;
                    //Амплитуда измерительного тока
                    double Im = ReadRegister(REG_ACTUAL_I_MEASUREMENT, true) / 10.0;
                    TopPanelVm.Im = Im;
                    //Температура корпуса
                    double TempCase1 = ReadRegister(REG_ACTUAL_T_CASE1, true) / 10.0;
                    double TempCase2 = ReadRegister(REG_ACTUAL_T_CASE2, true) / 10.0;
                    TopPanelVm.AnodeBodyTemperature = TempCase1;
                    TopPanelVm.CathodeBodyTemperature = TempCase2;
                    //Температура охладителя
                    double TempCool1 = ReadRegister(REG_ACTUAL_T_COOL1, true) / 10.0;
                    double TempCool2 = ReadRegister(REG_ACTUAL_T_COOL2, true) / 10.0;
                    TopPanelVm.AnodeCoolerTemperature = TempCool1;
                    TopPanelVm.CathodeCoolerTemperature = TempCool2;
                    //ТЧП
                    double Tsp = ReadRegister(REG_ACTUAL_TSP, true);
                    TopPanelVm.TemperatureSensitiveParameter = Tsp;
                    //Добавление точек на графики
                    Duration = AddPointsToChart(HeatingCurrent, HeatingPower, Tsp, TempCase1, TempCase2, TempCool1, TempCool2);
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not read data");
                }
                //Таймаут перед следующим запросом в миллисекундах
                int PollTimeout = 200;
                switch (Duration)
                {
                    //Менее 10 секунд
                    case double duration when duration >= 1 && duration < 10:
                        PollTimeout = 500;
                        break;
                    //Менее 100 секунд
                    case double duration when duration >= 10 && duration < 100:
                        PollTimeout = 1000;
                        break;
                    //Более 100 секунд
                    case double duration when duration >= 100:
                        PollTimeout = 3000;
                        break;
                }
                Thread.Sleep(PollTimeout);
            });
        }

        public void CheckDeviceState() //Проверка состояния
        {
            //Состояние
            HWDeviceState State = (HWDeviceState)ReadRegister(REG_DEV_STATE, true);
            if (!(State == HWDeviceState.DS_NONE || State == HWDeviceState.DS_INPROCESS || State == HWDeviceState.DS_READY))
            {
                IsRunning = false;
                App.Logger.Error("Device is in fault state");
                HWFaultReason Reason = (HWFaultReason)ReadRegister(REG_FAULT_REASON);
                MessageBox.Show(string.Format("Код ошибки: {0}", Reason), "Ошибка на комплексе");
                //Закрытие приложения
                App.Current.Dispatcher.BeginInvoke(() =>
                {
                    App.Current.MainWindow.Close();
                });
            }
            switch (CommonVM)
            {
                //Zth длинный импульс
                case ZthLongImpulseVM LongImpulseVM:
                    //Состояние операции
                    HWOperationState OpState = (HWOperationState)ReadRegister(REG_OP_STATE, true);
                    //Нагрев завершен
                    if (OpState == HWOperationState.OPSTATE_MEASURING)
                    {
                        LongImpulseVM.StopHeatingButtonIsEnabled = false;
                        LongImpulseVM.StartHeatingButtonIsEnabled = false;
                        LongImpulseVM.StopMeasurementButtonIsEnabled = true;
                        LongImpulseVM.RightPanelTextBoxsIsEnabled = false;
                    }
                    break;
                //Zth последовательность
                case ZthPulseSequenceVM PulseSequenceVM:
                    HWResult Result = (HWResult)ReadRegister(REG_OP_RESULT, true);
                    if (Result == HWResult.OPRESULT_OK)
                    {
                        if (IsRunning == true)
                            ReadEndpointsZthSequence();
                        IsRunning = false;
                        BottomPanelVM.LeftButtonIsEnabled = true;
                        BottomPanelVM.RightButtonIsEnabled = true;
                        PulseSequenceVM.StopMeasurementButtonEnabled = false;
                        PulseSequenceVM.LineSeriesCursorLeftVisibility = true;
                    }
                    break;
                //Градуировка
                case GraduationOnlyVM GraduationOnlyVM:
                    //Состояние операции
                    HWOperationState OpStateG = (HWOperationState)ReadRegister(REG_OP_STATE, true);
                    //Нагрев завершен
                    if (OpStateG == HWOperationState.OPSTATE_MEASURING)
                    {
                        GraduationOnlyVM.StopHeatingButtonIsEnabled = false;
                        GraduationOnlyVM.StartHeatingButtonIsEnabled = false;
                        GraduationOnlyVM.StopGraduationButtonIsEnabled = true;
                        GraduationOnlyVM.RightPanelTextBoxsIsEnabled = false;
                        GraduationOnlyVM.AmplitudeControlCurrentTextBoxIsEnabled = false;
                        GraduationOnlyVM.DurationHeatingCurrentPulseTextBoxIsEnabled = false;
                    }
                    break;
            }
        }

        public double AddPointsToChart(double heatingCurrent, double heatingPower, double tsp, double tempCase1, double tempCase2, double tempCool1, double tempCool2) //Добавление точек на графики
        {
            //Измерение не запущено
            if (!IsRunning)
                return 0;
            double Duration = (DateTime.Now - StartTime).TotalSeconds;
            //Время
            CommonVM.Time = Duration;
            //Греющий ток
            CommonVM.HeatingCurrent = heatingCurrent;
            CommonVM.HeatingCurrentChartValues.Add(new ObservablePoint(Duration, heatingCurrent));
            //Греющая мощность
            CommonVM.HeatingPower = heatingPower;
            CommonVM.HeatingPowerChartValues.Add(new ObservablePoint(Duration, heatingPower));
            //ТЧП
            CommonVM.TemperatureSensitiveParameter = tsp;
            if (CommonVM.TemperatureSensitiveParameterIsVisibly)
                CommonVM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(Duration, tsp));
            //Температура корпуса
            CommonVM.AnodeBodyTemperature = tempCase1;
            CommonVM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(Duration, tempCase1));
            CommonVM.CathodeBodyTemperature = tempCase2;
            if (CommonVM.CathodeBodyTemperatureIsVisibly)
                CommonVM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(Duration, tempCase2));
            //Температура охладителя
            CommonVM.AnodeCoolerTemperature = tempCool1;
            CommonVM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(Duration, tempCool1));
            CommonVM.CathodeCoolerTemperature = tempCool2;
            if (CommonVM.CathodeCoolerTemperatureIsVisibly)
                CommonVM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(Duration, tempCool2));
            if ((int)Duration % 3 == 0)
                //Выравнивание графиков
                App.Current.Dispatcher.BeginInvoke(() =>
                {
                    Chart.AdjustChart();
                });
            return Duration;
        }

        public async void StopHeating() //Выключение нагрева
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Stopping the heating");
                    CallAction(ACT_STOP_HEATING);
                    App.Logger.Info("Heating stopped");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not stop heating");
                }
            });
            
        }

        public async Task StopProcess() //Выключение измерения
        {
            await Task.Run(() =>
            {
                try
                {
                    IsRunning = false;
                    App.Logger.Info("Stopping the process");
                    CallAction(ACT_STOP_PROCESS);
                    App.Logger.Info("Process stopped");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not stop the process");
                }
            });
            
        }

        #region Стандартные методы
        public ushort ReadRegister(ushort address, bool skipJournal = false) //Чтение значения регистра
        {
            ushort Result = Adapter.Read16(Node, address);
            if (!skipJournal)
                App.Logger.Info(string.Format("ReadRegister({0}) returned {1}", address, Result));
            return Result;
        }

        public void WriteRegister(ushort address, ushort value, bool skipJournal = false) //Запись значения регистра
        {
            Adapter.Write16(Node, address, value);
            if (!skipJournal)
                App.Logger.Info(string.Format("WriteRegister({0}, {1}) completed", address, value));
        }

        public void CallAction(ushort address, bool skipJournal = false) //Вызов функции
        {
            Adapter.Call(Node, address);
            if (!skipJournal)
                App.Logger.Info(string.Format("CallAction({0}) completed", address));
        }

        public IList<ushort> ReadArray(ushort address, bool skipJournal = false) //Чтение эндпоинтов
        {
            IList<ushort> Result = Adapter.ReadArray(Node, address);
            if (!skipJournal)
                App.Logger.Info(string.Format("ReadArray({0}) returned {1} values", address, Result.Count));
            return Result;
        }

        public void ClearFaults() //Очистка ошибок
        {
            CallAction(ACT_CLR_FAULT);
            App.Logger.Info("Faults cleared");
        }

        public void ClearWarnings() //Очистка предупреждений
        {
            CallAction(ACT_CLR_WARNING);
            App.Logger.Info("Warnings cleared");
        }
        #endregion

        #region Перечисления и регистры 
        internal enum HWDeviceState //Состояние
        {
            DS_NONE,
            DS_FAULT,
            DS_DISABLED,
            DS_POWERON,
            DS_READY,
            DS_INPROCESS
        }

        internal enum HWFaultReason //Причина ошибки
        {
            FAULT_NONE,
            FAULT_POWERON,
            FAULT_WATER,
            FAULT_TR1,
            FAULT_TR2,
            FAULT_REC,
            FAULT_NO_POT,
            FAULT_CUR_FOLLOWING_ERR
        }

        internal enum HWDisableReason //Причина выключения
        {
            DISABLE_NONE
        }

        internal enum HWWarningReason //Причина предупреждения
        {
            WARNING_NONE
        }

        internal enum HWResult //Результат операции
        {
            OPRESULT_NONE,
            OPRESULT_OK,
            OPRESULT_FAIL
        }

        internal enum HWOperationState //Состояние операции
        {
            OPSTATE_NONE,
            OPSTATE_HEATING,
            OPSTATE_MEASURING
        }

        internal const ushort
            //Функции
            ACT_ENABLE_POWER = 1,
            ACT_DISABLE_POWER = 2,
            ACT_CLR_FAULT = 3,
            ACT_CLR_WARNING = 4,
            ACT_START_PROCESS = 100,
            ACT_STOP_PROCESS = 101,
            ACT_STOP_HEATING = 102,
            ACT_UPDATE = 103,
            //Регистры
            REG_MODE = 128,
            REG_DUT_TYPE = 129,
            REG_COOLING_MODE = 130,
            REG_PULSE_WIDTH_MIN = 131,
            REG_PULSE_WIDTH_MAX_L = 132,
            REG_PULSE_WIDTH_MAX_H = 133,
            REG_ZTH_PAUSE = 134,
            REG_PAUSE = 135,
            REG_I_WIDTH_LESS_2MS = 136,
            REG_I_WIDTH_LESS_10MS = 137,
            REG_I_WIDTH_ABOVE_10MS = 138,
            REG_GATE_CURRENT = 139,
            REG_MEASURING_CURRENT = 140,
            REG_MEASUREMENT_DELAY = 141,
            REG_T_MAX = 142,
            REG_IGBT_V_GATE = 143,
            REG_DEV_STATE = 192,
            REG_FAULT_REASON = 193,
            REG_DISABLE_REASON = 194,
            REG_WARNING = 195,
            REG_PROBLEM = 196,
            REG_OP_RESULT = 197,
            REG_OP_STATE = 198,
            REG_ACTUAL_U_DUT = 200,
            REG_ACTUAL_I_DUT = 201,
            REG_ACTUAL_P_DUT_WHOLE = 202,
            REG_ACTUAL_P_DUT_FRACT = 203,
            REG_ACTUAL_I_MEASUREMENT = 206,
            REG_ACTUAL_T_CASE1 = 207,
            REG_ACTUAL_T_CASE2 = 208,
            REG_ACTUAL_T_COOL1 = 209,
            REG_ACTUAL_T_COOL2 = 210,
            REG_ACTUAL_TSP = 211,
            REG_ACTUAL_CAP_VOLTAGE = 212,
            //Эндпоинты
            EP_TSP = 1,
            EP_T_CASE1 = 2,
            EP_T_CASE2 = 3,
            EP_T_COOL1 = 4,
            EP_T_COOL2 = 5;
    }
    #endregion
}
