module TestHelpers

// Comparer logic found here:
// https://stackoverflow.com/questions/62251993/how-to-compare-two-lists-in-f
let rec compare xl yl = 
    match xl, yl with 
    | [], [] -> false
    | x::xs, y::ys -> true
    | _ -> failwith("First Assert Failed")
    
let assertScore createdScore expectedScore = 
    if(compare createdScore expectedScore) then
        ()
    else
        failwith("First Assert Failed")