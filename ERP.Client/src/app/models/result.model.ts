export class ResultModel<T>{
    data?: T;
    errorMessages?: string[];
    isSuccessFul: boolean = false;
}