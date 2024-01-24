// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using ArgsParser;
using ArgsParser.Attributes;
using ArgsParser.Interfaces;
using AterraEngine;
using AterraEngine.EngineLoader;

namespace AterraEngine_Workfloor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliCommandsAtlas : CliCommandAtlas {
    public class ArgOptions : ParameterOptions {
        [ArgFlag('h', "help")]      public bool ShowHelp { get; set; } = false;
        [ArgFlag('v', "version")]   public bool ShowVersion { get; set; } = false;
        [ArgFlag('d', "debug")]     public bool EnableDebug { get; set; } = false;
    
        [ArgFlag('r', "raylib")]    public bool RunRaylib { get; set; } = false;
        [ArgFlag('e', "editor")]    public bool RunEditor { get; set; } = false;
        [ArgFlag('c', "config")]    public bool RunConfig { get; set; } = false;
    
        [ArgValue('s', "something")] public string Something { get; set; } = "";
    }

    [CliCommand<ArgOptions>("test")]
    public void CallbackHelp(ArgOptions argOptions) {
        
    }

    [CliCommand<ArgOptions>("beta")]
    public void CallbackHelpa(ArgOptions argOptions) {
        Console.WriteLine($"BETA {argOptions.Something}, V:{argOptions.Verbose}");
    }
}


// ReSharper disable once UnusedType.Global
static class Program {
    public static void Main(string[] args) {
        new CliParser()
            .RegisterCli(new CliCommandsAtlas())
            .TryParse(args);
        
        var engineLoader = new EngineLoader<Engine2D>();
        var engine = engineLoader.CreateEngine();
        engine.Run();
    }
}


// // ---------------------------------------------------------------------------------------------------------------------
// // Imports
// // ---------------------------------------------------------------------------------------------------------------------
// using Raylib_cs;
//
// namespace AterraEngine_Workfloor;
//
// // ---------------------------------------------------------------------------------------------------------------------
// // Code
// // ---------------------------------------------------------------------------------------------------------------------
// public enum AnimalType {
//     Pig,
//     Cow,
//     Sheep
// }
//
// public abstract class Animal(string name, string noiseFilePath) {
//     // No rime or reason to why some are abstract and others not.
//     //      Just made some variations for the assignment
//     private string Name { get; } = name;
//     public abstract AnimalType AnimalType { get; }
//     public abstract string Noise { get; }
//     private Sound NoiseSound { get; } = Raylib.LoadSound(noiseFilePath);
//
//     public void CreateSound() {
//         Console.WriteLine($"The {AnimalType}, who is called '{Name}', says: '{Noise}'");
//         Raylib.PlaySound(NoiseSound);
//     }
// }
//
// public class Pig(string name) : Animal(name, "resources/audio/pig-1.wav") {
//     public override AnimalType AnimalType => AnimalType.Pig;
//     public override string Noise => "oink";
// }
//
// public class Cow(string name) : Animal(name, "resources/audio/cow-moo2.wav") {
//     public override AnimalType AnimalType => AnimalType.Cow;
//     public override string Noise => "Moo";
// }
//
// public class Sheep(string name) : Animal(name, "resources/audio/sheep-bah1.wav") {
//     public override AnimalType AnimalType => AnimalType.Sheep;
//     public override string Noise => "Baa";
// }
//
// public class Farm(IEnumerable<Animal> animals) {
//     private readonly List<Animal> _animals = animals.ToList();
//     public IReadOnlyList<Animal> Animals => _animals.AsReadOnly();
//     public int AnimalCount => Animals.Count;
//
//     public IEnumerable<Animal> GetByType(AnimalType animalType) {
//         return Animals.Where(a => a.AnimalType == animalType);
//     }
//     
//     public void AddAnimal(Animal animal) {
//         _animals.Add(animal);
//     }
//
//     public void LetTheAnimalsSpeak() {
//         var s = AnimalCount > 0 ? "s" : "";
//         Console.WriteLine($"There are {AnimalCount} ani" +
//                           $"mal{s}");
//         
//         foreach (Animal farmAnimal in _animals) {
//             farmAnimal.CreateSound();
//             Console.ReadLine();
//         }
//     }
//     
//     public static Farm Start() {
//         Farm farm = new Farm([
//             new Pig("Betty"),
//             new Cow("Bessy"),
//             // new Cow("Bessy2"),
//             // new Cow("Bessy3"),
//             // new Cow("Bessy4"),
//             // new Cow("Bessy5"),
//             // new Cow("Bessy6"),
//         ]);
//         farm.AddAnimal(new Sheep("Blue"));
//         
//         Console.WriteLine("\n \n \n \n \n \n \n \n \n "); // just some separation from Raylib stuff
//
//         farm.LetTheAnimalsSpeak();
//
//         return farm;
//     }
// }
//
// public class Program {
//     public static void Main() {
//         Raylib.InitAudioDevice();
//         
//         Farm farm = Farm.Start();
//     }
// }