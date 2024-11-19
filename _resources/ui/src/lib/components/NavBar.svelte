<script lang="ts">
	import { cn } from '$lib/utils.js';
	import Icon from '@iconify/svelte';

	let className: string | undefined | null = undefined;
	export { className as class };
</script>

<nav class={cn('flex items-center space-x-4 lg:space-x-6', className)}>
	<a
		href="/"
		class="flex flex-row flex-nowrap items-center gap-3 text-sm font-medium transition-colors hover:text-primary"
	>
		<Icon width="25" height="25" icon="ant-design:shop-twotone" />
		Home
	</a>
	<a href="/products" class="text-sm font-medium transition-colors hover:text-primary">
		Products
	</a>
</nav>
