///<amd-module name="utils/communication/rest"/>

import * as $ from "jquery";
import { App, AppSingleton } from "app";

export function get(controller: string, action: string): any {
    const url = `${App.apiUrl}/${controller}/${action}`;
    return $.ajax(url).always(xhr => {
        if (xhr.status === 401)
            AppSingleton.getInstance().unauthorize();
    });;
}

export function post(controller: string, action: string, body: any): any {
    const url = `${App.apiUrl}/${controller}/${action}`;
    return $.ajax({
        contentType: 'application/json',
        data: JSON.stringify(body),
        dataType: 'json',
        type: "POST",
        url: url
    }).always(xhr => {
        if (xhr.status === 401)
            AppSingleton.getInstance().unauthorize();
    });
}