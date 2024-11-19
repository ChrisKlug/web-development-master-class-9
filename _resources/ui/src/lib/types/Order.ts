export type AddOrderPayload = {
	items: Item[];
	deliveryAddress: Address;
	billingAddress: Address;
};

export type Item = {
	itemId: number;
	quantity: number;
};

export type Address = {
	name: string;
	street1: string;
	street2?: string;
	postalCode: string;
	city: string;
	country: string;
};
