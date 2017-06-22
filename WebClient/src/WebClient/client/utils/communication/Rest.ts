///<amd-module name="utils/communication/rest"/>

import * as $ from "jquery";
import { App } from "app";

export function get(controller: string, action: string): JQueryXHR {
    const url = `${App.apiUrl}/${controller}/${action}`;
    return $.ajax(url);
}