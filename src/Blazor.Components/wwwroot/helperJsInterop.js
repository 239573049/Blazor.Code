function openAssembly(name, DotNet) {
    // 读取缓存程序集 （注：只有在Https中才会使用）
    caches
        .match(name)
        .then(value => {
            value.arrayBuffer()
                .then(async (buffer) => {
                    await DotNet.invokeMethodAsync("RenderByte", (window.URL || window.webkitURL || window || {}).createObjectURL(new Blob([buffer])));
                })
        })
}

function revokeObjectURL(url) {
    (window.URL || window.webkitURL || window || {}).revokeObjectURL(url)
}



export {
    openAssembly,
    revokeObjectURL
}