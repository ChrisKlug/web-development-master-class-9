import { z } from 'zod';

export type ShoppingCart = {
	items: ShoppingCartItem[];
};

export type ShoppingCartItem = {
	productId: number;
	productName: string;
	price: number;
	count: number;
};

const address = z.object({
	name: z.string().min(4, 'Name must be at least 4 characters long'),
	street1: z.string().min(5, 'Street is required'),
	street2: z.string().optional(),
	postalCode: z.string().min(5, 'Postal code is required'),
	city: z.string().min(3, 'City is required'),
	country: z.string().min(3, 'Country is required')
});
export type CheckoutFormAddress = z.infer<typeof address>;

const item = z.object({
	itemId: z.number(),
	quantity: z.number()
});
export type CheckoutFormItem = z.infer<typeof item>;

export const checkoutFormSchema = z.object({
	items: z.array<typeof item>(item).min(1),
	deliveryAddress: address,
	billingAddress: address
});
export type CheckoutFormSchema = z.infer<typeof checkoutFormSchema>;
