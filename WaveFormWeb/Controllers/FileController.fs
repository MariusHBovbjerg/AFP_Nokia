namespace WaveFormWeb.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("[controller]")>]
[<ApiController>]
type FileController (logger : ILogger<FileController>) =
    inherit ControllerBase()

    [<HttpPost("produce")>]
    member this.Produce([<FromForm>]score:string) =
        match Assembler.assembleToPackedStream score with
            | Choice1Of2 ms -> WavePacker.write "sound" ms
            | Choice2Of2 err -> failwith err