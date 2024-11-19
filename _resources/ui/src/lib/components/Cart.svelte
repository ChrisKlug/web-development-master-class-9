<script lang="ts">
	import * as Popover from '$lib/components/ui/popover';
	import type { ShoppingCartItem } from '$lib/types/ShoppingCart';
	import Icon from '@iconify/svelte';
	import Badge from './ui/badge/badge.svelte';
	import Button from './ui/button/button.svelte';
	import Separator from './ui/separator/separator.svelte';

	let { cart }: { cart: ShoppingCartItem[] } = $props();
	let cartSum = $derived(cart.reduce((acc, item) => acc + item.price * item.count, 0));
	let contentLength = $derived(cart.reduce((acc, item) => acc + item.count, 0));
</script>

{#if cart && cart.length > 0}
	<Popover.Root>
		<Popover.Trigger>
			<div class="relative p-2">
				<Badge
					class="absolute right-0 top-0 rounded-full bg-green-600 px-1.5 py-0 text-[8px] leading-loose text-white"
					variant="outline">{contentLength}</Badge
				>
				<Icon icon="tdesign:cart" width={20} />
			</div>
		</Popover.Trigger>
		<Popover.Content class="w-[450px] bg-gray-100">
			<h3 class="mb-4 text-2xl font-bold">Cart</h3>
			{#if cart && cart.length > 0}
				<div class="grid grid-cols-[1fr_auto] gap-2">
					{#each cart as item (item.productId)}
						<span class="ml-4">{item.productName}</span>
						<p class="text-right">{item.count} x {item.price} SEK</p>
					{/each}
					<Separator class="col-span-2" />
					<p class="font-bold">Total:</p>
					<p class="text-right font-bold">
						{cartSum.toFixed(2)} SEK
					</p>
				</div>
				<div class="mt-4 flex w-full flex-row justify-end">
					<Popover.Close>
						<a href="/cart/checkout">
							<Button variant="default">Checkout</Button>
						</a>
					</Popover.Close>
				</div>
			{:else}
				<p class="text-lg font-bold">Your cart is empty</p>
			{/if}
		</Popover.Content>
	</Popover.Root>
{/if}
