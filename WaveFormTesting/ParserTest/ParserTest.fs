module ParserTest

open NUnit.Framework

open Parser

[<TestFixture>]
type ``The Parser Should Parse a simple score``() =

    [<Test>]
    member this.``It should parse a simple score`` () =
        let score = "32.#d3 16-"
        let result = parseScore score
        
        let assertFirstToken token = 
            if(token.length.duration = Thirtysecond && token.length.extendedDuration = true && token.pitch = Tone(DSharp, Three)) then
                ()
            else
                failwith("First Assert Failed")
        let assertSecondToken token = 
            if(token.length.duration = Sixteenth && token.length.extendedDuration = false && token.pitch = Rest) then
                ()
            else
                failwith("Second Assert Failed")

        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                List.head tokens |> assertFirstToken
                List.item 1 tokens |> assertSecondToken