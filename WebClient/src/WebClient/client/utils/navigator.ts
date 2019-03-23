///<amd-module name="utils/navigator"/>

export function navigate(path: string) {
    window.location.href = window.location.href.replace(/#(.*)$/, "") + "#" + path;
}