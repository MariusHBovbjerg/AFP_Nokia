module Parser

open FParsec

type Duration = Full | Half | Quarter | Eighth | Sixteenth | Thirtysecond
type Length = { duration: Duration; extendedDuration: bool }
type Note = A | ASharp | B | C | CSharp | D | DSharp | E | F | FSharp | G | GSharp
type Octave = One | Two | Three
type Pitch = Rest | Tone of note: Note * octave: Octave
type Token = { length: Length; pitch: Pitch }

let bpm = 120.
let secPerBeat = 60./bpm

let notes = [A;ASharp;B;C;CSharp;D;DSharp;E;F;FSharp;G;GSharp]

let octaveToInt octave =
    match octave with
        | One -> 1
        | Two -> 2
        | Three -> 3

let pExtendedParser : Parser<bool,Unit> = (stringReturn "." true) <|> (stringReturn "" false)

let measureDuration = (stringReturn "2" Half) <|> (stringReturn "4" Quarter) <|> (stringReturn "8" Eighth) <|> (stringReturn "16" Sixteenth) <|> (stringReturn "32" Thirtysecond) <|> (stringReturn "1" Full)

let getNoteLength = pipe2 measureDuration pExtendedParser (fun t e -> {duration = t; extendedDuration = e})

let isSharp = (stringReturn "#" true) <|> (stringReturn "" false)

let setSharp = pipe2 
                    isSharp
                    (anyOf "abcdefg") 
                    (fun isSharp note -> 
                        match (isSharp, note) with
                        | (false, 'a') -> A
                        | (true, 'a') -> ASharp    
                        | (false, 'b') -> B
                        | (false, 'c') -> C
                        | (true, 'c') -> CSharp
                        | (false, 'd') -> D
                        | (true, 'd') -> DSharp
                        | (false, 'e') -> E
                        | (false, 'f') -> F
                        | (true, 'f') -> FSharp
                        | (false, 'g') -> G
                        | (true, 'g') -> GSharp
                        | (_,unknown) -> sprintf "Unknown note %c" unknown |> failwith)

let evaluateOctave = anyOf "123" |>> (function
                | '1' -> One
                | '2' -> Two
                | '3' -> Three
                | unknown -> sprintf "Unknown octave %c" unknown |> failwith)

let evaluateTone = pipe2 setSharp evaluateOctave (fun n o -> Tone(note = n, octave = o))

let evaluateRest = stringReturn "-" Rest

let makeToken = pipe2 getNoteLength (evaluateRest <|> evaluateTone) (fun l p -> {length = l; pitch = p})

let pScore: Parser<Token list, Unit> = sepBy makeToken (pstring " ")

// Added ToLower() and Trim() to clean the input and guarantee that you can't mistype with uppercase by mistake
let parseScore (score: string): Choice<string, Token list> =
    match score.ToLower().Trim() |> run pScore with
        | Failure(errorMsg,_,_)-> Choice1Of2(errorMsg)
        | Success(result,_,_) -> Choice2Of2(result)

let durationFromToken (token: Token): float =
    (match token.length.duration with
        | Full -> 4.*1000.*secPerBeat
        | Half -> 2.*1000.*secPerBeat
        | Quarter -> 1.*1000.*secPerBeat
        | Eighth -> 1./2.*1000.*secPerBeat
        | Sixteenth -> 1./4.*1000.*secPerBeat
        | Thirtysecond -> (1./8.)*1000.*secPerBeat) 
        * (if token.length.extendedDuration then 1.5 else 1.0)

let overAllIndex (note, octave): int = 
    let noteIdx = List.findIndex (fun n -> n=note) notes
    noteIdx + ((octaveToInt octave - 1) * 12)

let semitonesBetween lower upper: int = 
    (overAllIndex upper) - (overAllIndex lower)

let frequency (token: Token): float = 
    match token.pitch with
        | Rest -> 0.
        | Tone (note,octave) ->
            let diff = semitonesBetween (A,One) (note,octave)
            220. * ((2. ** (1./12.)) ** (float diff)) 