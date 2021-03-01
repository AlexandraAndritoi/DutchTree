import * as _ from "lodash";
export class Order {
    constructor() {
        this.orderDate = new Date();
        this.items = new Array();
    }
    get subtotal() {
        return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
    }
    ;
}
export class OrderItem {
    constructor() {
        this.quantity = 0;
        this.unitPrice = 0;
    }
}
//# sourceMappingURL=order.js.map