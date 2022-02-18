module Assembler

    open System.IO
    open Parser
    open SignalGenerator
    open WavePacker
    
    let private tokenToSound token =
        generateSamples (durationFromToken token) (frequency token)
        |> Array.ofSeq
    
    let assemble tokens =
        List.map tokenToSound tokens |> Array.concat
 
    let assembleToPackedStream (score:string): Choice<string, MemoryStream> = 
        match parseScore score with
            | Choice1Of2 errorMsg -> Choice1Of2 errorMsg
            | Choice2Of2 tokens -> assemble tokens |> pack |> Choice2Of2