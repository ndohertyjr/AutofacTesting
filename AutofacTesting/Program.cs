// See https://aka.ms/new-console-template for more information

using Autofac;
using Autofac.Core;

namespace AutofacTesting;

public interface ILog
{
    void Write(string message);
}

public class ConsoleLog : ILog
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }
}

public class EmailLog : ILog
{
    private const string adminEmail = "admin@foo.com";
    
    public void Write(string message)
    {
        Console.WriteLine($"Email sent to {adminEmail} : {message}");
    }
}

public class SMSLog : ILog
{
    private string phoneNumber;

    public SMSLog(string phoneNumber)
    {
        this.phoneNumber = phoneNumber;
    }
    public void Write(string message)
    {
        Console.WriteLine($"SMS to {phoneNumber} : {message}");
    }
}

public class Engine
{
    private ILog log;
    private int id;
    private int speed;

    // Factory that returns object
    public delegate Engine Factory(int value);
    
    public Engine(ILog log, int value)
    {
        this.log = log;
        this.speed = value;
        id = new Random().Next();
        
    }

    public void Ahead(int power)
    {
        log.Write($"Engine [{id}] ahead {power}");
    }

    public void CheckSpeed()
    {
        log.Write($"Car is moving at speed {speed}");
    }
}

public class Car
{
    private Engine engine;
    private ILog log;

    public Car(Engine engine)
    {
        this.engine = engine;
        this.log = new EmailLog();
    }

    public Car(Engine engine, ILog log)
    {
        this.engine = engine;
        this.log = log;
    }

    public void Go()
    {
        engine.Ahead(100);
        log.Write("Car moving forward...");
    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = new ContainerBuilder();
        
        // Named parameters
//        builder.RegisterType<SMSLog>().As<ILog>()
//          .WithParameter("phoneNumber", "123-456-7890");
        
        // Typed parameter
//        builder.RegisterType<SMSLog>().As<ILog>()
//            .WithParameter(new TypedParameter(typeof(string), "123-456-7890"));
        
        // Resolved parameter
//        builder.RegisterType<SMSLog>().As<ILog>()
//            .WithParameter(
//                new ResolvedParameter(
//                    // Predicate
//                    (pi, ctx) => 
//                        pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
//                    //Value accessor
//                    (pi,ctx) => "123-456-7890"
//                    )
//                );

        // Factory process
        builder.RegisterType<Car>();
        builder.RegisterType<Engine>();
        builder.RegisterType<ConsoleLog>().As<ILog>();
        var container = builder.Build();
        
        var engineWithValueParameter = container.Resolve<Engine.Factory>();
        var engineWithVal10 = engineWithValueParameter(10);
        engineWithVal10.CheckSpeed();

        var engineWithVal20 = engineWithValueParameter(20);
        engineWithVal20.CheckSpeed();

    }
}