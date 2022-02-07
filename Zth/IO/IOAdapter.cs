using PE.SCCI;
using PE.SCCI.Master;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Zth.IO
{
    /// <summary>Ввод/вывод адаптера</summary>
    internal class IOAdapter : IDisposable
    {
        //Адаптер
        private readonly SCCIMasterAdapter Adapter;
        //Порт адаптера
        private readonly int Port;

        internal IOAdapter(int port = 1)
        {
            Adapter = new SCCIMasterAdapter(true);
            Port = port;
            App.Logger.Info(string.Format("Adapter created. Port: {0}", Port));
            Adapter_Initialize();
        }

        private void Adapter_Initialize() //Инициализация адаптера
        {
            App.Logger.Info("Connecting to Adapter");
            try
            {
                Adapter.Initialize(new SerialPortConfigurationMaster
                {
                    PortNumber = Port,
                    BaudRate = 115200,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    ParityMode = Parity.None,
                    TimeoutForSyncReceive = 3000,
                    TimeoutForSyncStreamReceive = 3000,
                    UseRetransmitsForArrays = true,
                    RetransmitsCountOnError = 5
                });
                App.Logger.Info("Connection to Adapter established");
            }
            catch
            {
                App.Logger.Error("Connection to Adapter failed");
            }
        }

        public void Dispose() //Освобождение ресурсов адаптера
        {
            App.Logger.Info("Disconnecting from Adapter");
            Adapter.Dispose();
            App.Logger.Info("Disconnection from Adapter established");
        }

        public ushort Read16(ushort node, ushort address) //Чтение значения регистра
        {
            try
            {
                return Adapter.Read16(node, address);
            }
            catch
            {
                throw;
            }
        }

        public void Write16(ushort node, ushort address, ushort value) //Запись значения регистра
        {
            try
            {
                Adapter.Write16(node, address, value);
            }
            catch
            {
                throw;
            }
        }

        public void Call(ushort node, ushort address) //Вызов функции
        {
            try
            {
                Adapter.Call(node, address);
            }
            catch
            {
                throw;
            }
        }

        public IList<ushort> ReadArray(ushort node, ushort address) //Чтение эндпоинтов
        {
            try
            {
                return Adapter.ReadArrayFast16(node, address);
            }
            catch
            {
                throw;
            }
        }
    }
}
