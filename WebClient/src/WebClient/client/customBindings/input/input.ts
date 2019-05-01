///<amd-module name="customBindings/input/input"/>

import * as ko from "knockout";

export function register(): void {
    ko.bindingHandlers.my_input = {
        init(element, valueAccessor, allBindings, viewModel, bindingContext) {
            $(element).addClass("my-input");
            
            const params: InputParameters = ko.unwrap(valueAccessor());

            element.onchange = (event) => {
                params.value(event.target.value);
                filled(element, params);
            }

            params.validate = () => {
                $(element).tooltip('hide');
                let message: string;
                let invalid = true;
                switch (true) {
                    case (params.require && params.value() === ""):
                        message = "This field cannot be empty";
                        break;
                    case (params.customValidate !== undefined && !params.customValidate(params.value())):
                        message = params.customValidationMsg;
                        break;
                    default:
                        invalid = false;
                }
                if (invalid) {
                    $(element).addClass("invalid");
                    if (message !== undefined) {
                        $(element).tooltip({
                            placement: 'right',
                            title: message,
                            trigger: 'manual',
                            template:
                                '<div class="tooltip input-invalid-message" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
                        });
                        $(element).tooltip('show');
                    }
                    return false;
                }
                $(element).removeClass("invalid");
                return true;
            }

            element.value = params.value();
            filled(element, params);
        },
        update(element, valueAccessor, allBindings, viewModel, bindingContext) {
        }
    }
}

function filled(element, params: InputParameters) {
    if (params.value() !== "")
        $(element).addClass("filled");
    if (params.value() === "")
        $(element).removeClass("filled");
}

export class InputParameters
{
    public value: ko.Observable<string>;
    public require?: boolean = false;
    public validate?: () => boolean;
    public customValidate?: (value: string) => boolean;
    public customValidationMsg?: string;
}