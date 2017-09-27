///<amd-module name="logIn/logIn"/>

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class LogIn {
    public readonly userName: ko.Observable<string>;
    public readonly password: ko.Observable<string>;

    private readonly onLogIn: () => void;

    constructor(onLogIn: () => void) {
        this.userName = ko.observable("");
        this.password = ko.observable("");
        this.onLogIn = onLogIn;
    }

    public validate = () => this.userName().length !== 0 && this.password().length !== 0;

    public logIn(): void {
        if (!this.validate()) return;

        rest.post("user", "logIn", { userName: this.userName(), password: this.password() })
            .done(() => this.onLogIn());
    }

    public dispose(): void {}
}
