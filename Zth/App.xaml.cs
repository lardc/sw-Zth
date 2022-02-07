using NLog;

namespace Zth
{
    public partial class App
    {
        /// <summary>Логгер приложения</summary>
        public static Logger Logger
        {
            get; set;
        } = LogManager.GetCurrentClassLogger();

        /// <summary>Логика взаимодействия с аппаратной частью</summary>
        internal static LogicContainer LogicContainer
        {
            get; set;
        } = new LogicContainer();
    }
}
