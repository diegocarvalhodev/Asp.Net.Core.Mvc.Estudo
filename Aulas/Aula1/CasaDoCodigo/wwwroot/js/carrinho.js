
class Carrinho {
    clickDecremento(btn) {
        let data = this.getData(btn);
        data.Quantidade--;
        this.postQuantidade(data);
    }

    clickIncremento(btn) {
        let data = this.getData(btn);
        data.Quantidade++;
        this.postQuantidade(data);
    }

    updateQuantidade(input) {
        let data = this.getData(input);
        this.postQuantidade(data);
    }

    getData(elemento) {
        var linhaDoItem = $(elemento).parents('[item-id]');
        var itemId = $(linhaDoItem).attr('item-id');
        var novaQtde = $(linhaDoItem).find('input').val();

        var data = {
            Id: itemId,
            Quantidade: novaQtde
        };
        return data;
    }

    postQuantidade(data) {
        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).done(function (response) {
            //location.reload();
            let itemPedido = response.itemPedido;
            let linhaDoItem = $('[item-id=' + itemPedido.id + ']');
            linhaDoItem.find('input').val(itemPedido.quantidade);
            linhaDoItem.find('[subtotal]').html( (itemPedido.subtotal).duasCasas() );

            let carrinhoViewModel = response.carrinhoViewModel;

            if (carrinhoViewModel.itens.length > 1) {
                $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');
            }
            else {
                $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' item');
            }

            $('[total]').html((carrinhoViewModel.total).duasCasas());

            if (itemPedido.quantidade == 0) {
                linhaDoItem.remove();
            }

            debugger;
        });
    }
}

var carrinho = new Carrinho();

Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}