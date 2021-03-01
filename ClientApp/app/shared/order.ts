import * as _ from "lodash";

export class Order {
    orderId: number | undefined;
    orderDate: Date = new Date();
    orderNumber: string | undefined;
    items: Array<OrderItem> = new Array<OrderItem>();

    get subtotal(): number {
        return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
    };
}

export class OrderItem {
    id: number | undefined;
    quantity: number = 0;
    unitPrice: number = 0;
    productId: number | undefined;
    productCategory: string | undefined;
    productSize: string | undefined;
    productTitle: string | undefined;
    productArtist: string | undefined;
    productArtId: string | undefined;
}