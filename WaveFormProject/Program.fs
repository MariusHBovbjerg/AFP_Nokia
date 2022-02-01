// Learn more about F# at http://fsharp.org

open System
open HelloWorldExample
open SignalGenerator

[<EntryPoint>]
let main argv =
    let duration = 3.
    let freq = 1000.

    let signalSeq = SignalGenerator.generateSamples (duration * 1000.) freq
    let signalArr = Seq.toArray(signalSeq)
    let res = WavePacker.pack(signalArr)
    let _ = WavePacker.writeSingle "test" res
    0 // return an integer exit code
