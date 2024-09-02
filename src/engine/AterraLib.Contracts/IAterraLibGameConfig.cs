using AterraCore.Contracts.ConfigMancer;

namespace AterraLib.Contracts;
public interface IAterraLibGameConfig : IConfigMancerElement {
    string SomePupperty { get; set; }
}
