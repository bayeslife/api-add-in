
package nz.co.API.Technical.API1;

import java.util.ArrayList;
import java.util.List;
import javax.annotation.Generated;
import javax.validation.Valid;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.apache.commons.lang3.builder.ToStringBuilder;


/**
 * Dessert
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "oranges",
    "ingredients"
})
public class Dessert {

    /**
     * 
     * 
     */
    @JsonProperty("oranges")
    @Valid
    private List<Orange> oranges = new ArrayList<Orange>();
    /**
     * 
     * 
     */
    @JsonProperty("ingredients")
    @Valid
    private List<Ingredients> ingredients = new ArrayList<Ingredients>();

    /**
     * 
     * 
     * @return
     *     The oranges
     */
    @JsonProperty("oranges")
    public List<Orange> getOranges() {
        return oranges;
    }

    /**
     * 
     * 
     * @param oranges
     *     The oranges
     */
    @JsonProperty("oranges")
    public void setOranges(List<Orange> oranges) {
        this.oranges = oranges;
    }

    /**
     * 
     * 
     * @return
     *     The ingredients
     */
    @JsonProperty("ingredients")
    public List<Ingredients> getIngredients() {
        return ingredients;
    }

    /**
     * 
     * 
     * @param ingredients
     *     The ingredients
     */
    @JsonProperty("ingredients")
    public void setIngredients(List<Ingredients> ingredients) {
        this.ingredients = ingredients;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(oranges).append(ingredients).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof Dessert) == false) {
            return false;
        }
        Dessert rhs = ((Dessert) other);
        return new EqualsBuilder().append(oranges, rhs.oranges).append(ingredients, rhs.ingredients).isEquals();
    }

}
