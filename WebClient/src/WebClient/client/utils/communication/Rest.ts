///<amd-module name="utils/communication/rest"/>

import * as $ from "jquery";
import { App, AppSingleton } from "app";

export function get(controller: string, action: string): any {
    const url = `${App.apiUrl}/${controller}/${action}`;
    return $.ajax({
        headers: {
            "Authorization": "bearer " + readToken()
        },
        url: url
    }).always(xhr => {
        if (xhr.status === 401)
            AppSingleton.getInstance().unauthorize();
    });;
}

export function post(controller: string, action: string, body: any): any {
    const url = `${App.apiUrl}/${controller}/${action}`;
    return $.ajax({
        contentType: 'application/json',
        data: JSON.stringify(body),
        type: "POST",
        headers: {
            "authorization": "bearer " + readToken()
        },
        url: url
    }).always(xhr => {
        if (xhr.status === 401)
            AppSingleton.getInstance().unauthorize();
    });
}

export function ping(): any {
    return get("buildings", "");
}

function readToken(): string {
    var nameEQ = "bubelsoftToken=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}