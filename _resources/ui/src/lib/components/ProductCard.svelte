<script lang="ts">
	import { addToCart } from '$lib/api/cart/addToCart';
	import { Button } from '$lib/components/ui/button';
	import * as Card from '$lib/components/ui/card';
	import type { Product } from '$lib/types/Product';
	import Icon from '@iconify/svelte';
	import { toast } from 'svelte-sonner';
	import type { ButtonEventHandler } from 'bits-ui';
	import { invalidate } from '$app/navigation';

	let { product }: { product: Product } = $props();

	async function handleAddToCard(event: ButtonEventHandler) {
		event.preventDefault();
		try {
			await addToCart(fetch, product.id, 1);
			toast.success('Product added to cart');
			invalidate('app:cart');
		} catch (error) {
			toast.error(JSON.stringify(error));
		}
	}
</script>

<Card.Root class="h-full">
	<Card.Content class="group relative flex h-full flex-col">
		<div
			class="aspect-h-1 aspect-w-1 lg:aspect-none lg:h-50 w-full overflow-hidden rounded-md group-hover:opacity-75"
		>
			<img
				class="h-full w-full object-contain object-center lg:h-full lg:w-full"
				src={product.imageUrl}
				alt={product.name}
			/>
		</div>
		<div class="mt-4 grid grid-cols-[1fr_auto] gap-2">
			<h3 class="text-sm text-gray-700">
				<a href="/products/{product.id}">
					<span aria-hidden="true" class="absolute inset-0"></span>
					{product.name}
				</a>
			</h3>
			<p class="whitespace-nowrap text-sm font-medium text-gray-900">
				{product.price} SEK
			</p>
			<p class="col-span-2 mt-1 text-xs text-gray-500">{product.description}</p>
		</div>
		<div class="z-50 mt-3 flex flex-grow justify-center">
			<Button on:click={handleAddToCard} variant="default" class="self-end hover:bg-green-600">
				<Icon icon="bx:cart-add" class="mr-2 h-4 w-4" />
				<p>Add to cart</p>
			</Button>
		</div>
	</Card.Content>
</Card.Root>
