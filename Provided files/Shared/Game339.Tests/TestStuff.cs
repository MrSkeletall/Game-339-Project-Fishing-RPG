using Game339.Shared;
using Game339.Shared.Models;

namespace Game339.Tests;

public class TestStuff
{
    public abstract class Animal
    {
        public abstract void Speak();

        public virtual void SpeakX()
        {
            Console.WriteLine("I must scream but I have no implementation");
        }
        
        public bool IsAlive()
        {
            return true;
        }
    }
    
    public class Dog : Animal
    {
        public override void Speak()
        {
            Console.WriteLine("Woof!");
        }
    }

    public class ShibaInu : Dog
    {
        public override void Speak()
        {
            Console.WriteLine("Meow");
            base.Speak();
        }
    }
    
    [Test]
    public void MyTest()
    {
        var c = new Character();

        Console.WriteLine("Starting to set value to 5");
        c.Health.Value = 5;
        Console.WriteLine("Done setting value to 5");

        c.Health.ChangeEvent += HealthOnChangeEvent1;
        c.Health.ChangeEvent += HealthOnChangeEvent2;
        c.Health.ChangeEvent -= HealthOnChangeEvent1;

        Console.WriteLine("Starting to set value to 10");
        c.Health.Value = 10;
        c.Health.Value = 10;
        Console.WriteLine("Done setting value to 10");
        
        Assert.That(c.Health.Value, Is.EqualTo(10));
    }

    private void HealthOnChangeEvent1(int obj)
    {
        Console.WriteLine("ONE: Hey, I'm changing! to " + obj);
    }
    
    
    private void HealthOnChangeEvent2(int obj)
    {
        Console.WriteLine("TWO: Hey, I'm changing! to " + obj);
    }
}