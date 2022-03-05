namespace WaveFormWeb.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type HomeController (logger : ILogger<HomeController>) =
    inherit Controller()

    member this.Index () =
        this.View()