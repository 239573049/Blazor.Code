function openAssembly(name, DotNet) {
    // ��ȡ������� ��ע��ֻ����Https�вŻ�ʹ�ã�
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