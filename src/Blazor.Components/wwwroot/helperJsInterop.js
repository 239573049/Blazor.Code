export function openAssembly(name, DotNet) {
    console.log("²âÊÔ")
    caches.match(name)
        .then(value => {
            value.arrayBuffer()
                .then(blob => {
                    await DotNet.invokeMethodAsync("RenderByte", blob)
                    //var url = (window.URL || window.webkitURL || window || {}).createObjectURL(new Blob([blob]))
                    //console.log(url, new Blob([blob]))
                })
        })
}