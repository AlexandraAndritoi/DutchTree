class StoreCustomer {

    constructor(private firstName:string, private lastName:string) {
    }

    public visits: number = 0;

    showName() {
        alert(this.firstName + " " + this.lastName);
    }
}