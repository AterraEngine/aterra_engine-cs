using Ascii.BorderLibrary;
using Ascii.Library;
using CodeOfChaos.Ansi;

var ansiShadow = new AnsiShadow();
// Console.WriteLine(ansiShadow.ConvertToAsciiString("Aterra Engine"));
//
// Console.ReadKey();
//
// ansiShadow = new AnsiShadow(config => {
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("hotpink"), '█','▄' );
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("aqua"), '╔','═', '╗', '║', '╚', '╝' );
// });
// Console.WriteLine(ansiShadow.ConvertToAsciiString("Aterra Engine"));
//
// Console.ReadKey();
//
// ansiShadow = new AnsiShadow(config => {
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("darkslategray"), '█','▄' );
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("lightslategray"), '╔','═', '╗', '║', '╚', '╝' );
//     
//     config.RegisterBorder<ThinBorder>();
// });
// Console.WriteLine(ansiShadow.ConvertToAsciiString("Aterra Engine"));
//
// Console.ReadKey();
//
// ansiShadow = new AnsiShadow(config => {
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("darkslategray"), '█','▄' );
//     config.RegisterAnsiMarkup(AnsiColor.AsFore("lightslategray"), '╔','═', '╗', '║', '╚', '╝' );
//     
//     config.RegisterBorder<ThinPadding1Border>();
// });
// Console.WriteLine(ansiShadow.ConvertToAsciiString(" Aterra Engine "));
// Console.WriteLine(ansiShadow.ConvertToAsciiString(" A "));
//
// Console.ReadKey();

ansiShadow = new AnsiShadow(config => {
    config.RegisterAnsiMarkup(AnsiColor.AsFore("darkslategray"), '█','▄' );
    config.RegisterAnsiMarkup(AnsiColor.AsFore("lightslategray"), '╔','═', '╗', '║', '╚', '╝' );
    
    config.RegisterBorder(ThinBorder.GeneratePaddingBorder(1));
    // config.RegisterBorder<ThinBorder>();
});
Console.WriteLine(ansiShadow.ConvertToAsciiString(" Aterra Engine "));
Console.WriteLine(ansiShadow.ConvertToAsciiString(" A "));
