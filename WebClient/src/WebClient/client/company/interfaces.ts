﻿interface ICompany extends ICompanyInfo {
    canManageWorkers: boolean;
    workers: IUser[];
}

interface ICompanyInfo {
    id: string;
    name: string;
    nip: string;
    phoneNumber: string;
    email: string;
    city: string;
    postCode: string;
    street: string;
    placeNumber: string;
    canEdit: boolean;
}

interface IUser {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    companyRole: UserCompanyRole | string | ISelectValue;
    forRegister: boolean;
}

enum UserCompanyRole {
    Admin,
    UserAdmin,
    Worker
}

interface ISelectValue {
    value: number;
    displayValue: string;
}

enum UserBuildingRole {
    Admin,
    Reporter
}

interface IMap {
    key: UserBuildingRole;
    value: string;
}