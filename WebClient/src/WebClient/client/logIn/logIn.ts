///<amd-module name="logIn/logIn"/>

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class LogIn {
    public readonly userName: ko.Observable<string>;
    public readonly password: ko.Observable<string>;
    public readonly invalid: ko.Observable<boolean>;

    private readonly onLogIn: () => void;

    constructor(onLogIn: () => void) {
        this.userName = ko.observable("");
        this.password = ko.observable("");
        this.invalid = ko.observable(false);
        this.onLogIn = onLogIn;
    }

    public validate = () => this.userName().length !== 0 && this.password().length !== 0;

    public logIn(): void {
        if (!this.validate()) {
            this.invalid(true);
            return;
        }

        rest.post("user", "logIn", { userName: this.userName(), password: this.password() })
            .done((data: ITokenInfo) => {
                this.setCookies(data)
                this.onLogIn();
                this.invalid(false);
            })
            .fail(() => this.invalid(true));
    }

    public dispose(): void { }

    private setCookies(data: ITokenInfo): void
    {
        document.cookie = "bubelsoftToken=" + data.token + "; expires=" + data.expiration + "; path=/";
    }
}

interface ITokenInfo {
    token: string;
    expiration: Date;
}
