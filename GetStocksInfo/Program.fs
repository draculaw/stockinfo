// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
//
//# read from start
//# start  http://finance.yahoo.com/q/hp?s=600568.SS
//
//# if the next exist read the next
//# next <a rel="next" href="/q/hp?s=600568.SS&amp;d=3&amp;e=14&amp;f=2015&amp;g=d&amp;a=4&amp;b=18&amp;c=2001&amp;z=66&amp;y=66" data-rapid_p="21">Next</a>
//
//# <body><div><div><table id="yfncsumtab"><tbody><tr><td><table i"yfnc_datamodoutline1"><tbody><tr><td><table><tbody><tr>
//
//# <tr id="yui_3_9_1_9_1428976539885_38"><td class="yfnc_tabledata1" nowrap="" align="right">Apr 13, 2015</td><td class="yfnc_tabledata1" align="right">20.22</td><td class="yfnc_tabledata1" align="right">21.25</td><td class="yfnc_tabledata1" align="right">20.22</td><td class="yfnc_tabledata1" align="right">20.49</td><td class="yfnc_tabledata1" align="right">14,455,400</td><td class="yfnc_tabledata1" align="right">20.49</td></tr>

//    next_url = doc.xpath("//a[@rel='next']")

//    data = doc.xpath("//td[@class='yfnc_tabledata1']/..")

open System.IO
open System.Net
open Microsoft.FSharp.Control.WebExtensions

let url =  "http://finance.yahoo.com" + "600563.SS"

let fetchAsync(name, url:string) =
    async {
        try 
            let uri = new System.Uri(url)
            let webClient = new WebClient()
            let! html = webClient.AsyncDownloadString(uri)
            printfn "data is %s" (html.ToString())
            //printfn "%s is %s" name url
            
        with
            | ex -> printfn "Exception: %s" ex.Message
    }

let urlList = ["600805", "http://finance.yahoo.com/q/hp?s=600568.SS"
               "600503", "http://finance.yahoo.com/q/hp?s=600503.SS"]
let getAllData(urllist) =
    urllist
    |> Seq.map fetchAsync
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

let getCodeFromFile(name:string) =
    try
        let content = File.ReadAllLines(name)
        content |> Array.toList
    with
        | :? System.IO.FileNotFoundException as e -> 
        printfn "exception %s" e.Message; ["empty"]

[<EntryPoint>]
let main argv = 
    getCodeFromFile "sz_code.txt"
    |> printfn

    0 // return an integer exit code
