///<amd-module name="start/login/login"/>

import { InputParameters } from "customBindings/input/input";
import * as ko from "knockout";
import * as rest from "utils/communication/rest";
import * as navigator from "utils/navigator";

export class Login {
    private userEmail: ko.Observable<string>;
    private userPassword: ko.Observable<string>;
    private errorMessage: ko.Observable<string>;

    public userEmailParams: ko.Observable<InputParameters>;
    public userPasswordParams: ko.Observable<InputParameters>;

    constructor() {
        this.userEmail = ko.observable("");
        this.userPassword = ko.observable("");
        this.errorMessage = ko.observable("");


        this.userEmailParams = ko.observable<InputParameters>(
            {
                value: this.userEmail,
                require: true
            });
        this.userPasswordParams = ko.observable<InputParameters>(
            {
                value: this.userPassword,
                require: true
            });
    }

    public login() {
        this.errorMessage("");

        let valid = this.userEmailParams().validate();
        valid = this.userPasswordParams().validate() && valid;
        if (!valid)
            return;

        rest.post("user",
            "login",
            {
                email: this.userEmail(),
                password: this.userPassword()
            })
            .done((data: ITokenInfo) => {
                document.cookie = "bubelsoftToken=" + data.token + "; expires=" + new Date(data.expiration) + "; path=/";
                navigator.navigate("home");
            })
            .fail((data: any) => this.errorMessage(data.responseText));
    }
}

interface ITokenInfo {
    token: string;
    expiration: string;
}