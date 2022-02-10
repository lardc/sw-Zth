using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        public LogicContainer(ushort node = 1)
        {
            Adapter = new IOAdapter();
            Node = node;
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
                    while ((HWDeviceState)ReadRegister(REG_DEV_STATE) != HWDeviceState.DS_READY)
                        Thread.Sleep(50);
                    App.Logger.Info("Power enabled");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Enabling power failed");
                }
            });
        }

        public async void DisablePower() //Выключение установки
        {
            await Task.Run(() =>
            {
                try
                {
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

        public async void StartZthLongImpulse(ushort pulseWidth) //Запуск измерения Zth длинный импульс
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 8);
                    ushort LowByte = (ushort)(pulseWidth & 0x00FF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Starting Zth long impulse");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Zth long impulse started");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Zth long impulse");
                }
            });
        }

        public async void UpdateZthLongImpulse(TypeDevice device, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay, ushort pulseWidth) //Обновление параметров измерения Zth длинный импульс
        {
            await Task.Run(() =>
            {
                try
                {
                    //Обновление общих параметров
                    UpdateCommonData(device, gateParameter, measuringCurrent, heatingCurrent, measurementDelay);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 8);
                    ushort LowByte = (ushort)(pulseWidth & 0x00FF);
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

        public async void StartZthSequence(ushort firstPulseWidth, ushort lastPulseWidth, ushort pause) //Запуск измерения Zth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    App.Logger.Info("Setting the 1st pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MIN, firstPulseWidth);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(lastPulseWidth >> 8);
                    ushort LowByte = (ushort)(lastPulseWidth & 0x00FF);
                    App.Logger.Info("Setting the last pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_ZTH_PAUSE, pause);
                    App.Logger.Info("Starting Zth sequence");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Zth sequence started");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Zth sequence");
                }
            });
        }

        public async void StartRthSequence(ushort pulseWidth, ushort pause) //Запуск измерения Rth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 8);
                    ushort LowByte = (ushort)(pulseWidth & 0x00FF);
                    App.Logger.Info("Setting pulse width");
                    WriteRegister(REG_PULSE_WIDTH_MAX_L, LowByte);
                    WriteRegister(REG_PULSE_WIDTH_MAX_H, HighByte);
                    App.Logger.Info("Setting pulse pause");
                    WriteRegister(REG_PAUSE, pause);
                    App.Logger.Info("Starting Rth sequence");
                    CallAction(ACT_START_PROCESS);
                    App.Logger.Info("Rth sequence started");
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not start Rth sequence");
                }
            });
        }

        public async void UpdateRthSequence(TypeDevice device, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay, ushort pulseWidth, ushort pause) //Обновление параметров измерения Rth последовательность
        {
            await Task.Run(() =>
            {
                try
                {
                    //Обновление общих параметров
                    UpdateCommonData(device, gateParameter, measuringCurrent, heatingCurrent, measurementDelay);
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 8);
                    ushort LowByte = (ushort)(pulseWidth & 0x00FF);
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

        public async void StartGraduation(ushort pulseWidth, ushort pause, ushort temperature) //Запуск измерения Градуировка
        {
            await Task.Run(() =>
            {
                try
                {
                    ClearFaults();
                    ClearWarnings();
                    //Старший и младший байты длительности импульса
                    ushort HighByte = (ushort)(pulseWidth >> 8);
                    ushort LowByte = (ushort)(pulseWidth & 0x00FF);
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

        public async void PrepareForMeasure(TypeDevice device, TypeCooling cooling, WorkingMode mode, ushort gateParameter, ushort measuringCurrent, ushort[] heatingCurrent, ushort measurementDelay) //Подготовка к измерению
        {
            await Task.Run(() =>
            {
                try
                {
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
                    App.Logger.Info("Setting measurment delay");
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
            App.Logger.Info("Setting measurment delay");
            WriteRegister(REG_MEASUREMENT_DELAY, measurementDelay);
        }

        public async void ReadResults(CommonVM vm) //Чтение результатов
        {
            await Task.Run(() =>
            {
                try
                {
                    App.Logger.Info("Reading the results");
                    //Греющий ток
                    ushort HeatingCurrent = (ushort)(ReadRegister(REG_ACTUAL_I_DUT) / 10);
                    //Греющая мощность
                    string HeatingPowerString = string.Format("{0}.{1}", ReadRegister(REG_ACTUAL_P_DUT_WHOLE), ReadRegister(REG_ACTUAL_P_DUT_FRACT) / 10);
                    ushort HeatingPower = ushort.Parse(HeatingPowerString);
                    //Температура корпуса
                    ushort TempCase1 = (ushort)(ReadRegister(REG_ACTUAL_T_CASE1) / 10);
                    ushort TempCase2 = (ushort)(ReadRegister(REG_ACTUAL_T_CASE2) / 10);
                    //Температура охладителя
                    ushort TempCool1 = (ushort)(ReadRegister(REG_ACTUAL_T_COOL1) / 10);
                    ushort TempCool2 = (ushort)(ReadRegister(REG_ACTUAL_T_COOL2) / 10);
                    //ТЧП
                    ushort Tsp = (ushort)(ReadRegister(REG_ACTUAL_TSP) / 10);                    
                }
                catch (Exception error)
                {
                    App.Logger.Error(error, "Could not read the results");
                }
            });
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

        public async void StopProcess() //Выключение измерения
        {
            await Task.Run(() =>
            {
                try
                {
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
            FAULT_REC
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
