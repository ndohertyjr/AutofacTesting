// See https://aka.ms/new-console-template for more information

using Autofac;

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

public class Engine
{
    private ILog log;
    private int id;

    public Engine(ILog log)
    {
        this.log = log;
        id = new Random().Next();
    }

    public void Ahead(int power)
    {
        log.Write($"Engine [{id}] ahead {power}");
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
        builder.RegisterType<ConsoleLog>().As<ILog>();
        
        // Register a second Type
        // builder.RegisterType<EmailLog>().As<ILog>().PreserveExistingDefaults();

        // Would use this particular instance of ConsoleLog
        // var log = new ConsoleLog();
        // builder.RegisterInstance(log).As<ILog>();
        
        //Register with lambda to specify other components to use
        builder.Register((c) => new Engine(c.Resolve<ILog>()));
        builder.RegisterType<Engine>();
        
        // Register Type and specify constructor
        //builder.RegisterType<Car>().UsingConstructor(typeof(Engine));
        builder.RegisterType<Car>();
        
        //Generics
        builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
        
        
        IContainer container = builder.Build();

        var car = container.Resolve<Car>();
        
        //Generics example for setting list to concrete type of string
        var myList = container.Resolve<IList<string>>();
        Console.WriteLine(myList.GetType());

        car.Go();
    }
}