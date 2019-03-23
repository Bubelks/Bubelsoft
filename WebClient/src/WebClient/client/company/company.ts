///<amd-module name="company/company"/>

import * as rest from "utils/communication/rest";
import * as ko from "knockout";

export class Company {
    public name: string;
    public workers: ko.ObservableArray<IUser>;
    public companyInfo: ko.Observable<ICompanyInfo>;
    public companyInfoEdit: ko.Observable<ICompanyInfo>;
    public userEdit: ko.Observable<IUser>;
    public userToDelete: number[];
    public canManageWorkers: ko.Observable<boolean>;
    public avaiableRoles: ISelectValue[];

    constructor(id: number) {
        this.companyInfo = ko.observable(null);
        this.companyInfoEdit = ko.observable(null);
        this.workers = ko.observableArray([]);
        this.userEdit = ko.observable(null);
        this.userToDelete = new Array();
        this.canManageWorkers = ko.observable(false);
        this.avaiableRoles = [
            {
                value: UserCompanyRole.Admin,
                displayValue: UserCompanyRolePresenter.getDisplayValue(UserCompanyRole.Admin)
            },
            {
                value: UserCompanyRole.UserAdmin,
                displayValue: UserCompanyRolePresenter.getDisplayValue(UserCompanyRole.UserAdmin)
            },
            {
                value: UserCompanyRole.Worker,
                displayValue: UserCompanyRolePresenter.getDisplayValue(UserCompanyRole.Worker)
            }
        ];

        this.companyRequest(id)
            .done((data: ICompany) => {
                this.companyInfo(data);
                this.companyInfoEdit(this.companyInfo());
                
                data.workers.forEach(w => w.companyRole = UserCompanyRolePresenter.getDisplayValue(w.companyRole));
                this.workers(data.workers);
                this.canManageWorkers(data.canManageWorkers);
            });
    }

    private companyRequest(id: number): JQueryPromise<ICompany> {
        if (id !== null)
            return rest.get("company", `${id}`);
        else
            return rest.get("company", "my");
        
    }

    public saveInfo(): void {
        rest.put("company", `${this.companyInfo().id}`, this.companyInfoEdit())
            .done(() => {
                this.companyInfo(this.companyInfoEdit());
            })
            .fail(() => this.companyInfoEdit(this.companyInfo()));
    }

    public saveUser(): void {
        var companyRole = (this.userEdit().companyRole as ISelectValue).value;
        this.userEdit().companyRole = companyRole;
        rest.put("company", `${this.companyInfo().id}/workers/add`, this.userEdit())
            .done(() => this.workers.push(this.userEdit()));
    }

    public confirmDelete(): void {
        rest.put("company", `${this.companyInfo().id}/workers/delete`, this.userToDelete)
            .done(() =>
                this.userToDelete.forEach(id => this.workers.remove(w => w.id == id)))
            .fail(() => this.cancelDelete());
    }

    public cancelDelete(): void {
        this.userToDelete = new Array();

        $("#delete-worker").removeClass("selected");
    }

    public toDelete = (user: IUser, event) => {
        var index = this.userToDelete.indexOf(user.id);
        if (index >= 0)
            this.userToDelete.splice(index, 1);
        else
            this.userToDelete.push(user.id);

        $(event.currentTarget).toggleClass("selected");
    }

    public openAddUser(): void {
        this.userEdit({
            id: 0,
            username: null,
            firstName: null,
            lastName: null,
            phoneNumber: null,
            email: null,
            companyRole: this.avaiableRoles.filter(r => r.value === UserCompanyRole.Worker)[0],
            forRegister: false
        });
    }

    public dispose(): void {
        
    }
}

class UserCompanyRolePresenter {
    public static getDisplayValue(role): string {
        switch (role) {
        case UserCompanyRole.Admin:
            return "Administrator";
        case UserCompanyRole.UserAdmin:
            return "User Administrator";
        default:
            return "Worker";
        }
    }
}