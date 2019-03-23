///<amd-module name="user/logIn/logIn"/>

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class LogIn {
    public readonly userName: ko.Observable<string>;
    public readonly password: ko.Observable<string>;
    public readonly invalid: ko.Observable<boolean>;
    public readonly loading: ko.Observable<boolean>;

    private readonly onLogIn: () => void;

    constructor(onLogIn: () => void) {
        this.userName = ko.observable("");
        this.password = ko.observable("");
        this.invalid = ko.observable(false);
        this.loading = ko.observable(false);
        this.onLogIn = onLogIn;
    }

    public validate = () => this.userName().length !== 0 && this.password().length !== 0;

    public logIn(): void {
        this.loading(true);

        if (!this.validate())
            this.invalid(true);
        else
            rest.post("user", "logIn", { userName: this.userName(), password: this.password() })
                .done((data: ITokenInfo) => {
                    this.setCookies(data);
                    this.onLogIn();
                    this.invalid(false);
                })
                .fail(() => this.invalid(true));

        this.loading(false);
        document.getElementsByTagName("login")[0].getElementsByTagName("button")[0].blur();
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
