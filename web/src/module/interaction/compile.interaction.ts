/**
 * 定义js互操对象（因为在ts不能有未定义但是当前方法是存在windo的）
 */
declare const DotNet;

const assemblyName = 'Blazor.Code.Shared';

class CompileInteraction {

    /**
     * 动态编辑razor
     * @param code 
     */
    async CompileRazor(code: string) {
        await DotNet.invokeMethodAsync(assemblyName, 'CompileRazor', code);
    }
}

// 这样就相当于静态方法全局只有一个
export default new CompileInteraction()