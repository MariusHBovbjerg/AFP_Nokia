namespace WaveFormWeb.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Mime
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Primitives

type HomeController (logger : ILogger<HomeController>) =
    inherit Controller()

    member this.Index () =
        this.View()

   (* [<HttpPost>]
    member this.Produce(score:string) =
        //let cd = ContentDisposition(FileName = "sound", Inline = true)
        match Assembler.assembleToPackedStream score with
            | Choice1Of2 ms -> WavePacker.writeSingle "sound"
            | Choice2Of2 err -> failwith err

            | Choice1Of2 ms ->
                this.Response.Headers.Add(
                    "Content-Disposition",
                    StringValues(cd.ToString()))
                ms.Position <- 0L
                this.File(ms, "audio/x-wav")
            | Choice2Of2 err -> failwith err*)