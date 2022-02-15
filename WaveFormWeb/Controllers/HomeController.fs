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