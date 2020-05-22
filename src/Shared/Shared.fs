namespace Shared

type Counter = { Value : int }


module Route =
    /// Defines how routes are generated on server and mapped from client
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ICounterApi =
    { GetInitialCounter : unit -> Async<Counter> }

