///<amd-module name="user/register/register"/>

import * as rest from "utils/communication/rest";
import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class RegisterUser {
    public readonly user: ko.Observable<IUserRegisterInfo>;
    public modalOpen: ko.Observable<boolean>;

    constructor(userId: number) {
        this.user = ko.observable(null);
        this.modalOpen = ko.observable(false);

        this.getUser(userId)
            .done(data => {
                this.user(data);
                this.user().forRegister = true;
                this.user().password = "";
                this.user().id = userId;
                this.openModal();
            });
    }

    public saveUser(): void {
        rest.post("user", "register", this.user())
            .done(() => {
                this.closeModal();
                navigator.navigate("home");
            });
    }

    private getUser(userId: number): JQueryPromise<IUserRegisterInfo> {
        return rest.get("user", `${userId}`);
    }

    private closeModal(): void {
        this.modalOpen(false);
        $("body").removeClass("modal-open");
        var modal = $("#registerModal");
        modal.removeClass("show");
        modal.css("display", "none");
    }

    private openModal(): void {
        this.modalOpen(true);
        $("body").addClass("modal-open");
    }

    public dispose(): void {
        
    }
}

interface IUserRegisterInfo extends IUser {
    password: string;
}