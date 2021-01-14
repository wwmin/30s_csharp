//namespace System.Threading.Tasks

//从 ValueTask => Task
async Task<int> ShowTaskAsync(){
    return await ValueTask.FromResult(1).AsTask();
}

(await ShowTaskAsync()).Dump();