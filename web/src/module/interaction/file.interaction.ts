/**
 * 定义js互操对象（因为在ts不能有未定义但是当前方法是存在windo的）
 */
declare const DotNet;

const assemblyName = 'Blazor.Code.Shared';

class FileInteraction {
    async CreateAsync(name: string, content: string) {
        await DotNet.invokeMethodAsync(assemblyName, 'CreateFile', name, content)
    }

    async ReadAsync(name: string) {
        return await DotNet.invokeMethodAsync(assemblyName, 'ReadFile', name)
    }

    async DeleteAsync(name: string) {
        await DotNet.invokeMethodAsync(assemblyName, 'DeleteFile', name)
    }

    async GetTreeAsync() {
        return DotNet.invokeMethodAsync(assemblyName, 'GetTreeFile')
    }
}

// 这样就相当于静态方法全局只有一个
export default new FileInteraction()