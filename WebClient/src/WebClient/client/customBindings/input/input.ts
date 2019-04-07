///<amd-module name="customBindings/input/input"/>

import * as ko from "knockout";

export function register(): void {
    ko.bindingHandlers.my_input = {
        init(element, valueAccessor, allBindings, viewModel, bindingContext) {
            $(element).addClass("my-input");
            const params: InputParameters = ko.unwrap(valueAccessor());
            element.onchange = (event) => {
                params.value(event.target.value);
                if (params.value() !== "")
                    $(element).addClass("filled");
                if (params.value() === "")
                    $(element).removeClass("filled");
            }

            params.validate = () => {
                if (params.require && params.value() === "") {
                    $(element).addClass("invalid");
                    return false;
                }
                $(element).removeClass("invalid");
                return true;
            }
        },
        update(element, valueAccessor, allBindings, viewModel, bindingContext) {

        }
    }
}

export class InputParameters
{
    public value: ko.Observable<string>;
    public require?: boolean = false;
    public validate?: () => boolean;
}