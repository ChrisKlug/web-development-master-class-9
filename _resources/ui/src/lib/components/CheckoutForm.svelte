<script lang="ts">
	import { addOrder } from '$lib/api/orders/addOrder';
	import * as Form from '$lib/components/ui/form';
	import { Input } from '$lib/components/ui/input';
	import { Separator } from '$lib/components/ui/separator';
	import {
		checkoutFormSchema,
		type CheckoutFormSchema,
		type ShoppingCartItem
	} from '$lib/types/ShoppingCart';
	import { isHttpError } from '@sveltejs/kit';
	import { toast } from 'svelte-sonner';
	import { superForm, type SuperValidated } from 'sveltekit-superforms';
	import { zodClient } from 'sveltekit-superforms/adapters';

	interface CheckoutFormProps {
		data: SuperValidated<CheckoutFormSchema>;
		cart: ShoppingCartItem[];
	}
	let { data, cart }: CheckoutFormProps = $props();
	let cartSum = $derived(cart.reduce((acc, item) => acc + item.price * item.count, 0));

	const form = superForm(data, {
		SPA: true,
		dataType: 'json',
		resetForm: true,
		clearOnSubmit: 'errors-and-message',
		validators: zodClient(checkoutFormSchema),
		validationMethod: 'auto',
		onUpdate: async ({ form, cancel }) => {
			if (form.valid) {
				try {
					await addOrder(fetch, form.data);
					toast.success('Order placed successfully, thank you!');
				} catch (error: unknown) {
					if (isHttpError(error)) {
						if (error.status === 401) {
							toast.error('You need to be logged in to place an order');
						} else {
							toast.error(error.body.message);
						}
					}
					cancel();
				}
			} else {
				toast.error('Please correct the errors in the form');
				cancel();
			}
		}
	});

	const { form: formData, message, enhance } = form;
</script>

<div class="container mx-auto max-w-4xl px-5 py-8 md:py-12">
	<div class="grid grid-cols-1 gap-8 md:grid-cols-2">
		<div class="space-y-6">
			{#if $message}<h3>{$message}</h3>{/if}
			<form method="POST" use:enhance>
				<h3 class="text-xl font-bold">Delivery Address</h3>
				<Separator class="my-2" />
				<Form.Field {form} name="deliveryAddress.name" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Name</Form.Label>
						<Input {...attrs} bind:value={$formData.deliveryAddress.name} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="deliveryAddress.street1" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Address</Form.Label>
						<Input {...attrs} bind:value={$formData.deliveryAddress.street1} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="deliveryAddress.street2" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Address 2</Form.Label>
						<Input {...attrs} bind:value={$formData.deliveryAddress.street2} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<div class="grid grid-cols-[1fr_2fr] gap-4">
					<Form.Field {form} name="deliveryAddress.postalCode" class="space-y-1">
						<Form.Control let:attrs>
							<Form.Label>Zip Code</Form.Label>
							<Input {...attrs} bind:value={$formData.deliveryAddress.postalCode} />
						</Form.Control>
						<Form.FieldErrors />
					</Form.Field>

					<Form.Field {form} name="deliveryAddress.city" class="space-y-1">
						<Form.Control let:attrs>
							<Form.Label>City</Form.Label>
							<Input {...attrs} bind:value={$formData.deliveryAddress.city} />
						</Form.Control>
						<Form.FieldErrors />
					</Form.Field>
				</div>

				<Form.Field {form} name="deliveryAddress.country" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Country</Form.Label>
						<Input {...attrs} bind:value={$formData.deliveryAddress.country} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<h3 class="mt-6 text-xl font-bold">Shipping Address</h3>
				<Separator class="my-2" />

				<Form.Field {form} name="billingAddress.name" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Name</Form.Label>
						<Input {...attrs} bind:value={$formData.billingAddress.name} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="billingAddress.street1" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Address</Form.Label>
						<Input {...attrs} bind:value={$formData.billingAddress.street1} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="billingAddress.street2" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Address 2</Form.Label>
						<Input {...attrs} bind:value={$formData.billingAddress.street2} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<div class="grid grid-cols-[1fr_2fr] gap-4">
					<Form.Field {form} name="billingAddress.postalCode" class="space-y-1">
						<Form.Control let:attrs>
							<Form.Label>Zip Code</Form.Label>
							<Input {...attrs} bind:value={$formData.billingAddress.postalCode} />
						</Form.Control>
						<Form.FieldErrors />
					</Form.Field>

					<Form.Field {form} name="billingAddress.city" class="space-y-1">
						<Form.Control let:attrs>
							<Form.Label>City</Form.Label>
							<Input {...attrs} bind:value={$formData.billingAddress.city} />
						</Form.Control>
						<Form.FieldErrors />
					</Form.Field>
				</div>

				<Form.Field {form} name="billingAddress.country" class="space-y-1">
					<Form.Control let:attrs>
						<Form.Label>Country</Form.Label>
						<Input {...attrs} bind:value={$formData.billingAddress.country} />
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<div class="flex w-full flex-row justify-end">
					<Form.Button class="mt-4" type="submit">Place order</Form.Button>
				</div>
			</form>
		</div>

		<div class="rounded-md bg-gray-100 p-4">
			<h3 class="mb-4 text-xl font-bold">Order Summary</h3>
			<div class="grid grid-cols-[1fr_auto] gap-2">
				{#if cart && cart.length > 0}
					{#each cart as item (item.productId)}
						<span class="ml-4">{item.productName}</span>
						<p class="text-right">{item.count} x {item.price} SEK</p>
					{/each}
					<Separator class="col-span-2" />
					<p class="font-bold">Total:</p>
					<p class="text-right font-bold">
						{cartSum.toFixed(2)} SEK
					</p>
				{/if}
			</div>
		</div>
	</div>
</div>
