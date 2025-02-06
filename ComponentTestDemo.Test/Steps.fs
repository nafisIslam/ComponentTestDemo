namespace ComponentTestDemo.Test

module Steps =
  open FSharp.Data
  open Expecto

  type Context = {
      Headers: (string*string) list
  }

  let host = "https://localhost:7242"

  let ``given`` = id
  let ``when`` = id
  let ``then`` = id
  let ``and`` = id

  let ``with auth`` () =
    { 
      Headers = [("Authorization", "Bearer bc329eaf-7de8-4017-a259-95adf7ca59de"); ("Content-Type", "application/json")];  
    }

  let ``without auth`` () =
    { 
      Headers = [("Content-Type", "application/json")]  
    }

  let ``remove auth`` context =
      { context with Context.Headers=List.filter ((<>) "Authorization" << fst) context.Headers }

  let ``with incorrect auth`` context =
      let context2  = ``without auth`` context
      { context2 with Context.Headers=("Authorization", sprintf "Bearer %A" (System.Guid.NewGuid ()))::context2.Headers }


  let sendPostRequest apiAction headers commandJson = 
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="POST", headers=headers, body= TextRequest commandJson, silentHttpErrors = true);

    match response.StatusCode with
      | 400 -> failwith "command failed"
      | _  -> response.Body

  let sendPostRequestExpectError apiAction headers commandJson = 
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="POST", headers=headers, body= TextRequest commandJson, silentHttpErrors = true);

    match response.StatusCode with
      | 400 -> response.Body
      | _  -> failwith "Should Not Be A Valid Request"

  let sendGetRequest apiAction headers = 
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="GET", headers=headers, silentHttpErrors = true);

    match response.StatusCode with
      | 400 -> failwith "command failed"
      | _  -> response.Body

  let sendGetRequestExpectError apiAction headers = 
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="GET", headers=headers, silentHttpErrors = true);

    match response.StatusCode with
      | 400 -> response.Body
      | _  -> failwith "Should Not Be A Valid Request"

  let firstOrDefault list =
    match list with
    | head :: tail -> head 
    | [] -> ""

  let clearDatabase () =
    let responseBody = sendPostRequest "ExecutionContext/CleanUp" [] "";
    match responseBody with
    | Text text ->
      let response = text |> JsonValue.Parse
      Expect.equal (response.GetProperty "message") (JsonValue.String "Database cleaned up successfully.") ""
      ()
    | _ -> failwith "Clean Up Failed"

  let testCaseWithCleanup description action =
    testCase description <| fun _ ->
      try
        clearDatabase ()
        action ()
      with
      | _ ->
          clearDatabase ()
          reraise ()

  let ftestCaseWithCleanup description action =
    ftestCase description <| fun _ ->
      try
        clearDatabase ()
        action ()
      with
        | _ ->
          clearDatabase ()
          reraise ()


