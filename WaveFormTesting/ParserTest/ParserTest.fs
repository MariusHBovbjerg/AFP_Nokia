module ParserTest

open NUnit.Framework

open Parser
open TestHelpers

[<TestFixture>]
type ``The Parser Should Parse Correctly``() =

    [<Test>]
    member this.``It should ignore leading and trailing spaces`` () =
        let score = "                 2b2 2B2                     "
        let result = parseScore score
        
        let token = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,Two)};
        
        let expectedScore = [token; token];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should ignore capitalization`` () =
        let score = "2b2 2B2"
        let result = parseScore score
        
        let token = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,Two)};
        
        let expectedScore = [token; token];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse all Notes`` () =
        let score = "2a1 2#a1 2b1 2c1 2#c1 2d1 2#d1 2e1 2f1 2#f1 2g1 2#g1"
        let result = parseScore score
        
        let aToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(A,One)};
        let aSharpToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(ASharp,One)};
        let bToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,One)};
        let cToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(C,One)};
        let cSharpToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(CSharp,One)};
        let dToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(D,One)};
        let dSharpToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(DSharp,One)};
        let eToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(E,One)};
        let fToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(F,One)};
        let fSharpToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(FSharp,One)};
        let gToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(G,One)};
        let gSharpToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(GSharp,One)};
        
        let expectedScore = [aToken; aSharpToken; 
                            bToken;
                            cToken; cSharpToken;
                            dToken; dSharpToken;
                            eToken;
                            fToken; fSharpToken;
                            gToken; gSharpToken];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(12, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse Rest`` () =
        let score = "2- 2b2"
        let result = parseScore score
        
        let restToken = { length = {duration = Half; extendedDuration = false}; pitch = Rest};
        let nonRestToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,Two)};
        
        let expectedScore = [restToken; nonRestToken];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse all Octaves`` () =
        let score = "2b1 2b2 2b3"
        let result = parseScore score
        
        let oneToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,One)};
        let twoToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,Two)};
        let threeToken = { length = {duration = Half; extendedDuration = false}; pitch = Tone(B,Three)};
        
        let expectedScore = [oneToken; twoToken; threeToken];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(3, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse extendedDuration`` () =
        let score = "2.b1 2-"
        let result = parseScore score
        
        let extendedToken = { length = {duration = Half; extendedDuration = true}; pitch = Tone(B,One)};
        let normalToken = { length = {duration = Half; extendedDuration = false}; pitch = Rest};
        
        let expectedScore = [extendedToken; normalToken];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse Sharp`` () =
        let score = "32.#d3 16-d3"
        let result = parseScore score
        
        let sharpToken = { length = {duration = Thirtysecond; extendedDuration = true}; pitch = Tone(DSharp, Three)};
        let notSharpToken = { length = {duration = Sixteenth; extendedDuration = false}; pitch = Tone(D, Three)};
        
        let expectedScore = [sharpToken; notSharpToken];
            
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(2, List.length tokens)
                assertScore tokens expectedScore
    
    [<Test>]
    member this.``It should parse all Durations`` () =
        let score = "32- 16- 8- 4- 2- 1-"
        let result = parseScore score
        
        let thirtySecondToken = { length = {duration = Thirtysecond; extendedDuration = false}; pitch = Rest};
        let sixteenthToken = { length = {duration = Sixteenth; extendedDuration = false}; pitch = Rest};
        let eighthToken = { length = {duration = Eighth; extendedDuration = false}; pitch = Rest};
        let quarterToken = { length = {duration = Quarter; extendedDuration = false}; pitch = Rest};
        let halfToken = { length = {duration = Half; extendedDuration = false}; pitch = Rest};
        let fullToken = { length = {duration = Full; extendedDuration = false}; pitch = Rest};
        
        let expectedScore = [thirtySecondToken; sixteenthToken; eighthToken; quarterToken; halfToken; fullToken];
                
        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(6, List.length tokens)
                assertScore tokens expectedScore

    [<Test>]
    member this.``It should parse a simple score`` () =
        let score = "32.#d3 16- 8b1 2e2"
        let result = parseScore score

        let token1 = { length = {duration = Thirtysecond; extendedDuration = true}; pitch = Tone(DSharp, Three)};
        let token2 = { length = {duration = Sixteenth; extendedDuration = false}; pitch = Rest};
        let token3 = { length = {duration = Eighth; extendedDuration = false}; pitch = Tone(B, One)};
        let token4 = { length = {duration = Half; extendedDuration = false}; pitch = Tone(E, Two)};

        let expectedScore = [token1; token2; token3; token4];

        match result with
            | Choice1Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice2Of2 tokens ->
                Assert.AreEqual(4, List.length tokens)
                assertScore tokens expectedScore