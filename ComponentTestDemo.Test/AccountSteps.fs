namespace ComponentTestDemo.Test

module AccountSteps = 
  open Steps
  open Expecto
  open FSharp.Data
  open FSharp.Data.JsonExtensions

  let ``user can register with username, email & password`` userName email password (context:Context) =
    let responseBody = 
      [|
          "userName", JsonValue.String userName
          "email", JsonValue.String email
          "password", JsonValue.String password
      |]
      |> JsonValue.Record
      |> string
      |> sendPostRequest "Authentication/register" context.Headers

    match responseBody with
    | Text text ->
      let response = text |> JsonValue.Parse
      Expect.equal (response.GetProperty "message") (JsonValue.String "User registered successfully.") "User registered successfully"
      context
    | _ -> failwith "Registration Failed"  

  let ``user can login`` email password userName (context:Context) =
    let responseBody = 
      [|
          "email", JsonValue.String email
          "password", JsonValue.String password
      |]
      |> JsonValue.Record
      |> string
      |> sendPostRequest "Authentication/Login" context.Headers

    match responseBody with
    | Text text ->
      let response = text |> JsonValue.Parse
      { context with Context.Headers=("Authorization", sprintf "Bearer %s" (response?token.AsString()))::context.Headers }
    | _ -> failwith "User Verification Failed" 

  let ``user logsout`` (context:Context) =
    let data = context.Headers |> List.filter (fun x -> (x |> fst) <> "Authorization")
    { context with Context.Headers= data } 

  let ``user tries to register with same email, he gets an error`` userName email password (context:Context) =
    let responseBody = 
      [|
          "userName", JsonValue.String userName
          "email", JsonValue.String email
          "password", JsonValue.String password
      |]
      |> JsonValue.Record
      |> string
      |> sendPostRequestExpectError "Authentication/Register" context.Headers

    match responseBody with
    | Text text ->
      let response = text |> JsonValue.Parse
      Expect.equal (response.GetProperty "message") (JsonValue.String "Email is already in use.") "Registration Cancled"
      context
    | _ -> failwith "With Same Email User Can Not Register Again"  


  let ``user is not loged in`` context = context

  let ``he can not access authorized api`` apiAction (context:Context) =
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="GET", headers=context.Headers, silentHttpErrors = true);
    Expect.equal response.StatusCode 401 "User Gets Unauthorized Error"
    context

  let ``he can access authorized api`` apiAction (context:Context) =
    let response = Http.Request(sprintf "%s/api/%s" host apiAction, httpMethod="GET", headers=context.Headers, silentHttpErrors = true);
    Expect.equal response.StatusCode 200 "User Gets Unauthorized Error"
    context

  let ``user registers then signs in`` (context:Context) =
    ``user can register with username, email & password`` "Nafis Islam" "nafis0014@example.com" "12345" context
      |> ``then``
      |> ``user can login`` "nafis0014@example.com" "12345" "Nafis Islam"

